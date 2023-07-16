using Microsoft.EntityFrameworkCore;
using Npgsql;
using Tickets.BAL.Dto;
using Tickets.DAL.Contexts;
using Tickets.DAL.Models;
using Tickets.BAL.Services.Interfaces;
using Tickets.BAL.Exceptions;
using Microsoft.Extensions.Options;
using Tickets.BAL.Options.Implementations;
using System.Text.RegularExpressions;
using Tickets.BAL.Common;
using AutoMapper;
/*
* 
* Сервис для работы с базой данных
* 
*/
namespace Tickets.BAL.Services.Implementations
{
    public class ProcessService : IProcessService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IOptions<SqlQueries> _sqlStorage;
        private const string conflictErrorMsg = "Database conflict error.";
        private const string databaseTimeoutErrorMsg = "Database timeout error.";
        private const string setLockTimeoutSqlName = "set_lock_timeout.sql";
        private const string updateRefundSegmentsSqlName = "update_refund_segments.sql";
        public ProcessService(ApplicationDbContext context, IOptions<SqlQueries> sqlStorage, IMapper mapper)
        {
            _context = context;
            _sqlStorage = sqlStorage;
            _mapper = mapper;
        }

        public async Task CreateSegmentsAsync(SaleRequestDto saleDto)
        {
            Route[] routes = saleDto.Routes.ToArray();
            Segments[] segments = new Segments[saleDto.Routes.Count()];
            for (int i = 0; i < segments.Length; i++)
            {
                segments[i] = _mapper.Map<Segments>(saleDto);
                _mapper.Map(routes[i], segments[i]);
                segments[i].ArriveDatetimeTimezone = $"-{routes[i].ArriveDatetime.Offset.Hours}";
                segments[i].ArriveDatetime = routes[i].ArriveDatetime.UtcDateTime;
                segments[i].OperationTimeTimezone = $"-{saleDto.OperationTime.Offset.Hours}";
                segments[i].OperationTime = saleDto.OperationTime.UtcDateTime;
                segments[i].DepartDatetimeTimezone = $"-{routes[i].DepartDatetime.Offset.Hours}";
                segments[i].DepartDatetime = routes[i].DepartDatetime.UtcDateTime;
                segments[i].SerialNumber = (uint)i + 1;
            }
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
                    var inEx = ex.InnerException;
                    if (inEx == null) throw;
                    if (inEx.InnerException is PostgresException inPgEx && inPgEx.SqlState == PostgresErrorCodes.LockNotAvailable)
                    {
                        throw new RequestTimeoutException(databaseTimeoutErrorMsg, inPgEx);
                    }
                    if (inEx is PostgresException pgex && pgex.SqlState == PostgresErrorCodes.UniqueViolation)
                    {
                        throw new ConflictException(conflictErrorMsg, pgex);
                    }
                    throw;
                }
            }
            await transaction.CommitAsync();
        }

        public async Task<bool> RefundSegmentsAsync(RefundRequestDto refundDto)
        {
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
                    var inEx = ex.InnerException;
                    if (inEx == null) throw;
                    if (inEx.InnerException is PostgresException inPgEx && inPgEx.SqlState == PostgresErrorCodes.LockNotAvailable)
                    {
                        throw new RequestTimeoutException(databaseTimeoutErrorMsg, inPgEx);
                    }
                    if (inEx is PostgresException pgex && pgex.SqlState == PostgresErrorCodes.UniqueViolation)
                    {
                        throw new ConflictException(conflictErrorMsg, pgex);
                    }
                    throw;
                }
                success = true;
            }
            return success;
        }
    }
}
