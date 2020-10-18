# SensorQualityEvaluator

## Problem Statement

Process the log file and automate the quality control evaluation based on pre-defined criteria in a controlled environment  

---

## Design Changes made in consideration of **Extendablity**, **Maintainablity** and **Distributed Modeling**

- The interface method name changed from **EvaluateLogFile**  -> **EvaluateLogFileContents**
- The following changes made to the format of the log file

```
reference temp:70.0 hum:45.0 mon:6
temp temp-1 2007-04-05T22:00 72.4
temp temp-1 2007-04-05T22:01 76.0
temp temp-1 2007-04-05T22:02 79.1
temp temp-1 2007-04-05T22:03 75.6
hum hum-1 2007-04-05T22:05 45.3
hum hum-1 2007-04-05T22:06 45.1
temp temp-1 2007-04-05T22:04 71.2
temp temp-2 2007-04-05T22:02 70.1
temp temp-2 2007-04-05T22:03 71.3
temp temp-1 2007-04-05T22:05 71.4
temp temp-1 2007-04-05T22:06 69.2
temp temp-1 2007-04-05T22:07 65.2
mon mon-2 2007-04-05T22:07 8
temp temp-1 2007-04-05T22:08 62.8
temp temp-1 2007-04-05T22:09 61.4
mon mon-2 2007-04-05T22:08 6
temp temp-1 2007-04-05T22:10 64.0
temp temp-1 2007-04-05T22:11 67.5
mon mon-1 2007-04-05T22:05 7
mon mon-1 2007-04-05T22:06 9
mon mon-2 2007-04-05T22:04 2
temp temp-1 2007-04-05T22:12 69.4
temp temp-2 2007-04-05T22:01 69.5
temp temp-2 2007-04-05T22:05 69.8
hum hum-1 2007-04-05T22:04 45.2
hum hum-2 2007-04-05T22:04 44.4
hum hum-2 2007-04-05T22:07 43.8
hum hum-2 2007-04-05T22:08 42.1
mon mon-1 2007-04-05T22:04 5
mon mon-2 2007-04-05T22:05 4
mon mon-2 2007-04-05T22:06 10
temp temp-2 2007-04-05T22:04 71.5
hum hum-2 2007-04-05T22:05 43.9
hum hum-2 2007-04-05T22:06 44.9
```

 1. Each line has complete information about the log it's describing and information required for processing a single log statement is not scattered in different parts of the file
 2. Since each line has complete information, the order does not matter. Log streaming from multiple distributed systems can easily flow into 1 place.
 3. Reference line changed to specify which reference value belongs to which sensor type `[sensorType: refValue]`
   
     - Adding a brand new sensorType: refvalue is pretty intutive and st. forward
     - Less error in sensorType reference values being represented just by the virtue of order. Order is a NO NO is distributed systems.
 4. Don't have to worry about any packet losses or missing log lines to cause completely wrong evaluations.
   


    > In the sample input provided in the problem statement, if one of the lines `[<type> > <name>]` goes missing, the entire evaluation for the sensors would go wrong  
    
---

>## **IMP**:  
>**Since there is no importance given to order, the output printed would have the correct JSON format and evalution results but would not follow any particular order**  
---

## 3rd Party Components Used (Why reinvent the wheel? :) )

- MathNet.Numerics nuget package (https://numerics.mathdotnet.com/)
- Extremely fast processing, and better memory performance string extension **SplitLines()** 
  -  The inbuilt string.Split() method uses pattern matching using regex which makes is slow for the specific scenario we have here where we just want to split into separate lines (pre-known separator '\r' '\n'). string.Split() also leads to a lot of allocations when the result is used in a foreach loop. Using the new ReadOnlySpan, we can avoid allocations and make the operation much faster by providing a enumerator.  
  Ref: https://www.meziantou.net/split-a-string-into-lines-without-allocation.htm
  - The enumeration on the result of SplitLines() is a ReadOnlySpan`<char`> which can be evaluted lazily as per need. So no pre-allocation of memory.  

## Assumptions:
- The first line of the logContents is always the reference line
- There is only 1 reference line per sample input
- During calculations 'within' is inclusive of the threshold value  

## Areas of improvement
- The threshold values for fault tolerance, standard deviation etc. can be moved to a config file and read from there instaed of hardcoded values
- The performance for evaluating the "Humidistats" and "CO Detectors" can be further improved. Right now all the readings of a particular humidistat or CO detector are being read into memory and then evaluated. But that can be avoided by doing inline evalution for those readings as they are being read and stop processing the readings as soon as a deviation from threshold is encountered.