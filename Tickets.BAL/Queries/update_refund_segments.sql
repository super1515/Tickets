UPDATE segments
SET operation_type = 'refund', operation_time = @operation_time, operation_time_timezone = @operation_time_timezone, operation_place = @operation_place
WHERE operation_type != 'refund' AND ticket_number = @ticket_number;