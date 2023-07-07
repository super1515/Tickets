using Microsoft.EntityFrameworkCore;
using Npgsql;
using Tickets.Application.Dto;
using Tickets.Infrastructure.Contexts;
using Tickets.Infrastructure.Models;
using Tickets.Infrastructure.Services.Interfaces;
using Tickets.Infrastructure.Exceptions;
using Microsoft.Extensions.Options;
using Tickets.WebAPI.Options.Implementations;
/*
* 
* Сервис для работы с базой данных
* 
*/
namespace Tickets.Infrastructure.Services.Implementations
{
    public class ProcessService : IProcessService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOptions<SqlQueries> _sqlStorage;
        private const string conflictSqlState = "23505";
        private const string timeoutSqlState = "55P03";
        private const string conflictErrorMsg = "Database conflict error.";
        private const string databaseTimeoutErrorMsg = "Database timeout error.";
        private const string setLockTimeoutSqlName = "set_lock_timeout.sql";
        private const string updateRefundSegmentsSqlName = "update_refund_segments.sql";
        public ProcessService(ApplicationDbContext context, IOptions<SqlQueries> sqlStorage)
        {
            _context = context;
            _sqlStorage = sqlStorage;
        }

        public async Task CreateSegmentsAsync(Segments[] segments)
        {
            if (segments == null || segments.Length == 0) throw new ArgumentException(nameof(segments));
            string setLockTimeoutSql = _sqlStorage.Value.GetBy(setLockTimeoutSqlName)!.Data;
            using var transaction = await _context.Database.BeginTransactionAsync();
            await _context.Database.ExecuteSqlRawAsync(setLockTimeoutSql);
            for (int i = 0; i < segments.Length; i++)
            {
                await _context.Segments.AddAsync(segments[i]);
                try
                {
                    await _context.SaveChangesAsync();
                }catch (DbUpdateException ex)
                {
                    PostgresException pge = ex.InnerException as PostgresException;
                    if (pge == null) throw;
                    if (pge.SqlState == timeoutSqlState)
                    {
                        throw new RequestTimeoutException(databaseTimeoutErrorMsg, pge);
                    }
                    if (pge.SqlState == conflictSqlState)
                    {
                        throw new ConflictException(conflictErrorMsg, pge);
                    }
                    throw;
                }
            }
            await transaction.CommitAsync();
        }

        public async Task<bool> RefundSegmentsAsync(RefundRequestDto refundDto)
        {
            if (refundDto == null) throw new ArgumentNullException(nameof(refundDto));
            bool success = false;
            string updateRefundSegmentsSql = _sqlStorage.Value.GetBy(updateRefundSegmentsSqlName)!.Data;
            NpgsqlParameter operationTimeParam = new NpgsqlParameter("@operation_time", refundDto.OperationTime.UtcDateTime);
            NpgsqlParameter operationTimeTimezoneParam = new NpgsqlParameter("@operation_time_timezone", $"-{refundDto.OperationTime.Offset.Hours}");
            NpgsqlParameter operationPlaceParam = new NpgsqlParameter("@operation_place", refundDto.OperationPlace);
            NpgsqlParameter ticketNumberParam = new NpgsqlParameter("@ticket_number", refundDto.TicketNumber);
            int rowAffectedCount = await _context.Database.ExecuteSqlRawAsync(updateRefundSegmentsSql, 
                operationTimeParam, operationTimeTimezoneParam, operationPlaceParam, ticketNumberParam);
            if (rowAffectedCount > 0)
            {
                try
                {
                    await _context.SaveChangesAsync();
                } catch (DbUpdateException ex) {
                    PostgresException pge = ex.InnerException as PostgresException;
                    if (pge == null) throw;
                    if (pge.SqlState == timeoutSqlState)
                    {
                        throw new RequestTimeoutException(databaseTimeoutErrorMsg, pge);
                    }
                    if (pge.SqlState == conflictSqlState)
                    {
                        throw new ConflictException(conflictErrorMsg, pge);
                    }
                    throw;
                }
                success = true;
            }
            return success;
        }
    }
}
