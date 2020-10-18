# SensorQualityEvaluator

## Problem Statement

Process the log file and automate the quality control evaluation based on pre-defined criteria in a controlled environment

## Design Changes made in consideration of extendablity, maintainablity and distributed modelling

I made the following changes to the format of the log file

```
reference temp:70.0 hum:45.0 mon:6
temp temp-1 2007-04-05T22:00 72.4
temp temp-1 2007-04-05T22:01 76.0
temp temp-1 2007-04-05T22:02 79.1
temp temp-1 2007-04-05T22:03 75.6
temp temp-1 2007-04-05T22:04 71.2
temp temp-1 2007-04-05T22:05 71.4
temp temp-1 2007-04-05T22:06 69.2
temp temp-1 2007-04-05T22:07 65.2
temp temp-1 2007-04-05T22:08 62.8
temp temp-1 2007-04-05T22:09 61.4
temp temp-1 2007-04-05T22:10 64.0
temp temp-1 2007-04-05T22:11 67.5
temp temp-1 2007-04-05T22:12 69.4
temp temp-2 2007-04-05T22:01 69.5
temp temp-2 2007-04-05T22:02 70.1
temp temp-2 2007-04-05T22:03 71.3
temp temp-2 2007-04-05T22:04 71.5
temp temp-2 2007-04-05T22:05 69.8
hum hum-1 2007-04-05T22:04 45.2
hum hum-1 2007-04-05T22:05 45.3
hum hum-1 2007-04-05T22:06 45.1
hum hum-2 2007-04-05T22:04 44.4
hum hum-2 2007-04-05T22:05 43.9
hum hum-2 2007-04-05T22:06 44.9
hum hum-2 2007-04-05T22:07 43.8
hum hum-2 2007-04-05T22:08 42.1
mon mon-1 2007-04-05T22:04 5
mon mon-1 2007-04-05T22:05 7
mon mon-1 2007-04-05T22:06 9
mon mon-2 2007-04-05T22:04 2
mon mon-2 2007-04-05T22:05 4
mon mon-2 2007-04-05T22:06 10
mon mon-2 2007-04-05T22:07 8
mon mon-2 2007-04-05T22:08 6
```

 1. Each line has complete information about the log it's describing and information required for processing a single log statement is not scattered in different parts of the file
 2. Since each line has complete information, the order does not matter. Log streaming from multiple distributed systems can easily flow into 1 place.
 3. Reference line changed to specify whi reference value belongs to which sensor type `[sensorType: refValue]`
   
           - Adding a new sensorType: refvalue is pretty intutive and st. forward
           - Less error in order of values being represented just by the virtue   of order. Order is a NO NO is distributed systems.
 4. Don't have to worry about any packet losses or missing log lines to cause completely wrong evaluations.
   


    > In the sample input provided in the problem statement, if one of the lines `[<type> > <name>]` goes missing, the entire evaluation for the sensors would go wrong