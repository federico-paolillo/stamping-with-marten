# Stamping

A stamping system using [event sourcing](https://martinfowler.com/eaaDev/EventSourcing.html) built with [Marten](https://martendb.io/)

## Overview

The example features following events:

- Employee hired
- Employee stamped In
- Employee stamped out
- Employee stamping corrected
- Employee stamping deleted

These events are then aggregated in two projections:

- Timesheet
- Working day

Timesheet is the consolidation (i.e.: with corrections and deletions already factored in) of all stamping entries for an employee.  
Working day is the aggregation of the working hours of all the consolidate stamping entires of a given day for an employee.  

# Details

Projections are produced using `TimesheetProjection` and `WorkingDayProjection` classes, [according to Marten documentation](https://martendb.io/events/projections/aggregate-projections.html#applying-changes-to-the-aggregate-document).  
In order to consolidate the whole stream of events into stampings the whole stream must be considered because corrections and deletions events have "pointers" to the timestamp event to correct or delete and any correction or deletion can change a timestamp in the past.  
Such characteristics of events require that projection keep "memory" of the stampings seen to be able to resolve any pointer on a correction or deletion. An example of this "memory" is [the `Stampings` collection on `WorkingDay`](https://github.com/federico-paolillo/stamping-with-marten/blob/a2bc7ae363bd5249bba717b31990c1669fbbd583/Stampings/Models/WorkingDay.cs#L11), without this collection it would be impossible to apply a correction or deletion on the aggregated hours as it would not be possible to know how much the total will need to change.    
Because Marten does not support parametric projections (i.e.: passing custom values to projections before running them) we are forced to store any parametrizion on the aggregate itself. An example of this fact is how [the `Day` property on `WorkingDay` is used`](https://github.com/federico-paolillo/stamping-with-marten/blob/a2bc7ae363bd5249bba717b31990c1669fbbd583/Stampings/Projections/WorkingDayProjection.cs#L15).  