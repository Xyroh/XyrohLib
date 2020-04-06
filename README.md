# XyrohLib
A cross platform app support library, interfacing to ticket systems, crash reporting, and logging frameworks.  It's a library we use internally across a raft of C# projects on multiple platforms (.net / Xamarin / Console) etc.

Not currently finished, polished, but watch this space and use and enhance as you will and create a PR, anything that fits the bill will get approved and added to the master branch.

## Release Notes

#### Version 1.0.0
* Log file recycles at 1MB or at a customisable int (bytes) value, eg

```
XyrohLib.setFileLog("debug.log", 1000000); //1MB
```
* Can get full log file path back from Lib

```
XyrohLib.setFileLog("test.log");
XyrohLib.GetLogPath()); ///Users/Dev/Xyroh/XyrohLib/XyrohLib/ConsoleTest/bin/Debug/netcoreapp2.0/test.log
```


## Feature Requests
For things we're working on ourselves, or to request we add support please head over to our [Ideas Board](https://xyroh.atlassian.net/servicedesk/customer/portal/2/topic/46876d1c-e6a6-4fbb-8234-371c3259fecb/article/6488195) to see the state of play and make a request

## Support
This is not a commercial project, so very much on your own, but if you get stuck feel free to raise a [Support Ticket](https://xyroh.atlassian.net/servicedesk/customer/portal/2/topic/46876d1c-e6a6-4fbb-8234-371c3259fecb) and we'll see what we can do on a best endeavours basis

### Build Status
[![Master](https://xyroh.visualstudio.com/Xyroh%20Build%20Projects/_apis/build/status/Xyroh.XyrohLib?branchName=master)](https://xyroh.visualstudio.com/Xyroh%20Build%20Projects/_build/latest?definitionId=12&branchName=master)

[![Develop](https://xyroh.visualstudio.com/Xyroh%20Build%20Projects/_apis/build/status/Xyroh.XyrohLib?branchName=develop)](https://xyroh.visualstudio.com/Xyroh%20Build%20Projects/_build/latest?definitionId=12&branchName=develop)
