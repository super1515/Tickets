UPDATE segments
SET operation_type = 'refund', operation_time = @operation_time, operation_time_timezone = @operation_time_timezone
WHERE operation_type != 'refund' AND operation_place = @operation_place AND ticket_number = @ticket_number;