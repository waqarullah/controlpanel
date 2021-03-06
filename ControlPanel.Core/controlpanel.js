﻿var viewModel = function (entityList) {
    var self = this;
    var templist = [];
    for (var i = 0; i < entityList.length; i++) {
        templist.push(new SystemProcess(entityList[i]));
        
    }
    self.selectedSystemProcess=ko.observable(),
    self.systemProcesses = ko.observableArray(templist),
    self.eventLogs = ko.observableArray([]),
        self.updateProcessThreads = function (threads) {
            var logs = [];
            var threadIDs = [];
            for (var i = 0; i < threads.length; i++) {
                threadIDs.push(threads[i].SystemProcessThreadId);
            }
            for (var i = 0; i < self.systemProcesses().length; i++) {
                for (var j = 0; j < self.systemProcesses()[i].systemProcessThreadList().length; j++) {
                    var thread = getThreadByThreadID(self.systemProcesses()[i].systemProcessThreadList()[j].systemProcessThreadId(), threads);
                    if (thread) {
                        if (thread.ShowUpdateInLog) {
                            self.addLog(new EventLog("Last Update Message: " + thread.Message, EventLogTypes.Log));
                        }
                        if (thread.EstimatedExecutionTime && thread.ExecutionTime && thread.EstimatedExecutionTime < thread.ExecutionTime) {
                            self.addLog(new EventLog("Execution Time Exeeded By (" + (thread.ExecutionTime - thread.EstimatedExecutionTime) + "): " + thread.Message, EventLogTypes.Warning));
                        }
                        self.systemProcesses()[i].systemProcessThreadList()[j].updateThread(thread);
                    }
                }
            }
            return logs;
        };
    self.addLog = function (log) {
        log.message(new Date().toISOString() + '>' + log.message());
        if (self.eventLogs.length > 100) {
            self.eventLogs.shift();
        }
        self.eventLogs.push(log);
        $("#divActivity").scrollTop($("#divActivity").prop('scrollHeight'));
    },
    self.currentThread = ko.observable(),
      self.setCurrentThread = function (item) {
          $("#newthreadpanel").css({ 'display': 'none' });
          var scTime =item.continuousScheduleTime();
          if (scTime.indexOf("At") >= 0) {
              var newStr = scTime.replace("At ", "");
              var a = newStr.split(":");
              var min;
              var Hours
              if (a[0] > 9) {
                  Hours = a[0];
              }
              else { Hours = "0" + a[0] }
              if (a[1] > 9) {
                  min = a[1];
                  var temp = Hours + ":" + min + ":00";
                  document.getElementById("ScheduledTime").value = temp;
                  item.displaytime(temp);

              }
              else {
                  min = a[1];
                  min = "0" + min;
                  var temp = Hours + ":" + min + ":00";
                  document.getElementById("ScheduledTime").value = temp;
                  item.displaytime(temp);

              }
              item.displaytime = temp;
              self.currentThread(item);
          }
          else {
              self.currentThread(item);
          }
 
          self.selectedSystemProcess(item.systemProcessId());
      }


    self.removethread = ko.observable(),
        self.setremovethread = function (item) {
        self.removethread(item);
        var id = item.systemProcessThreadId();
        var u = getUrlParts(document.URL);
        var pathurl = u.pathname;
        $.ajax({
            url: pathurl+'/removeEntries',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ id: id }),
            success: function (result) {
                location.reload();
            }
        });
        },
    self.ThreadChanged = function ()
    {
        $('#newthreadpanel').slideUp("slow");
    }
    
    return { systemProcesses: self.systemProcesses, updateProcessThreads: self.updateProcessThreads, addLog: self.addLog, eventLogs: self.eventLogs, setCurrentThread: self.setCurrentThread, currentThread: self.currentThread, selectedSystemProcess: self.selectedSystemProcess, setremovethread:self.setremovethread, removethread:self.removethread,ThreadChanged:self.ThreadChanged };
}

var SystemProcess = function (entity) {
    var templist = [];
    var self = this;
    for (var i = 0; i < entity.SystemProcessThreadList.length; i++) {
        templist.push(new SystemProcessThread(entity.SystemProcessThreadList[i]));
    }
    self.systemProcessId = ko.observable(entity.SystemProcessId || ''),
        self.name = ko.observable(entity.Name || ''),
        self.description = ko.observable(entity.Description || ''),
        self.enabled = ko.observable(entity.Enabled || ''),
        self.displayOrder = ko.observable(entity.DisplayOrder || ''),
        self.systemProcessThreadList = ko.observableArray(templist || '');
        self.ip = ko.observable(entity.Ip || '');
    
    return { systemProcessThreadList: self.systemProcessThreadList, name: self.name, systemProcessId: self.systemProcessId, ip: self.ip };
}

var SystemProcessThread = function (entity) {
    var self = this;
    self.systemProcessThreadId = ko.observable(entity.SystemProcessThreadId || ''),
    self.showUpdateInLog = ko.observable(entity.ShowUpdateInLog || ''),
        self.systemProcessId = ko.observable(entity.SystemProcessId || ''),
        self.name = ko.observable(entity.Name || ''),
        self.lastsuccessfullyexecuted = ko.observable(entity.LastSuccessfullyExecutedStr || ''),
        self.LastExecutedSeconds = ko.observable(entity.LastExecutedSeconds || ''),
        self.springEntryName = ko.observable(entity.SpringEntryName || ''),
        self.description = ko.observable(entity.Description || ''),
        self.enabled = ko.observable(entity.Enabled || ''),
        self.continuous = ko.observable(entity.Continuous || ''),
        self.sleepTime = ko.observable(entity.SleepTime || ''),
        self.autoStart = ko.observable(entity.AutoStart || ''),
        self.status = ko.observable(entity.Status || ''),
        self.message = ko.observable(entity.Message || ''),
        self.scheduledTime = ko.observable(entity.ScheduledTime || ''),
        
 
    self.displaytime = ko.computed({
        read: function () {
            return self.scheduledTime().Hours + ':' + self.scheduledTime().Minutes;
        },
        write: function (value) {
            value = value;
        }
    });
    self.startRange = ko.observable(entity.StartRange || ''),
    self.endRange = ko.observable(entity.EndRange || ''),
    self.lastSuccessfullyExecutedStr = ko.observable(entity.LastSuccessfullyExecutedStr || ''),
    self.continuousDelay = ko.observable(entity.ContinuousDelay || ''),
    //self.ExecutionTime = ko.computed(function () {
    //    return parseInt(entity.ExecutionTime);
    //});

    self.ExecutionTime = ko.computed({
        read: function () {
            return parseInt(entity.ExecutionTime);
        },
        write: function (value) {
            value = parseInt(entity.ExecutionTime);
        }
    });

        self.isDeleted = ko.observable(entity.IsDeleted || ''),
        self.displayOrder = ko.observable(entity.DisplayOrder || ''),
        self.argument = ko.observable(entity.Argument || ''),
        self.lastUpdateDateStr = ko.observable(entity.LastUpdateDateStr || ''),
        self.continuousButtonText = ko.computed(function () {
            return self.continuous() ? "Stop" : "Start";
        }),
        self.enableButtonText = ko.computed(function () {
            return self.enabled() ? "Stop" : "Start";
        }),
      
        self.continuousScheduleTime = ko.computed(function () {
            return self.continuous() == true ? ((self.scheduledTime()) ? "At " + self.scheduledTime().Hours + ":" + self.scheduledTime().Minutes : "Every " + self.continuousDelay()) : "";
        });
        self.updateThread = function (entity) {
        self.ExecutionTime(entity.ExecutionTime || '');
        self.systemProcessThreadId(entity.SystemProcessThreadId || '');
        self.systemProcessId(entity.SystemProcessId || '');
        self.name(entity.Name || '');
        self.showUpdateInLog(entity.ShowUpdateInLog || '');
        self.springEntryName(entity.SpringEntryName || '');
        self.description(entity.Description || '');
        self.enabled(entity.Enabled || '');
        self.continuous(entity.Continuous || '');
        self.sleepTime(entity.SleepTime || '');
        self.autoStart(entity.AutoStart || '');
        self.status(entity.Status || '');
        self.message(entity.Message || '');
        self.scheduledTime(entity.ScheduledTime || '');
        self.startRange(entity.StartRange || '');
        self.endRange(entity.EndRange || '');
        self.lastSuccessfullyExecutedStr(entity.LastSuccessfullyExecutedStr || '');
        self.LastExecutedSeconds(entity.LastExecutedSeconds || '');
        self.continuousDelay(entity.ContinuousDelay || '');
        self.isDeleted(entity.IsDeleted || '');
        self.displayOrder(entity.DisplayOrder || '');
        self.argument(entity.Argument || '');
        self.lastUpdateDateStr(entity.LastUpdateDateStr || '');
      
        
    }
        return {
            ExecutionTime: self.ExecutionTime,
            LastExecutedSeconds : self.LastExecutedSeconds,
            lastsuccessfullyexecuted: self.lastSuccessfullyExecutedStr,
            name: self.name,
            systemProcessThreadId: self.systemProcessThreadId,
            systemProcessId: self.systemProcessId,
            message: self.message,
            scheduledTime: self.scheduledTime,
            displayOrder: self.displayOrder,
            lastSuccessfullyExecutedStr: self.lastSuccessfullyExecutedStr,
            enabled: self.enabled,
            continuous: self.continuous,
            continuousDelay: self.continuousDelay,
            status: self.status,
            enableButtonText: self.enableButtonText,
            continuousButtonText: self.continuousButtonText,
            continuousScheduleTime: self.continuousScheduleTime,
            updateThread: self.updateThread,
            showUpdateInLog: self.showUpdateInLog,
            springEntryName: self.springEntryName,
            description: self.description,
            argument: self.argument,
            displaytime: self.displaytime,
            dspdate: self.dspdate,
            
    };
}

var EventLogTypes = EventLogTypes || {};
EventLogTypes.Error = 1;
EventLogTypes.Warning = 2;
EventLogTypes.Log = 3;


var EventLog = function (msg, logType) {
    var self = this;
    self.message = ko.observable(msg);
    self.logType = ko.observable(logType);
    self.cssClass = ko.computed(function () {
        if (self.logType() == EventLogTypes.Error) return 'error';
        else if (self.logType() == EventLogTypes.Log) return 'log';
        else if (self.logType() == EventLogTypes.Warning) return 'warning';
    });
    return { message: self.message, logType: self.logType, cssClass: self.cssClass };
}

function getThreadByThreadID(threadID, threads) {
    for (var i = 0; i < threads.length; i++) {
        if (threads[i].SystemProcessThreadId == threadID)
            return threads[i];
    }
    return null;
}

function getUrlParts(url) {
    var a = document.createElement('a');
    a.href = url;

    return {
        href: a.href,
        host: a.host,
        hostname: a.hostname,
        port: a.port,
        pathname: a.pathname,
        protocol: a.protocol,
        hash: a.hash,
        search: a.search
    };
}