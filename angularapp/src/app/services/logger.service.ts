import { Injectable } from '@angular/core';
import { NGXLogger } from 'ngx-logger';
import { HttpClient, HttpHeaders } from '@angular/common/http'; // Import HttpHeaders
import { CommonService } from './common.service';
import { User } from './user.service';

export interface ClientInfo {

  ipAddress?: string;
  hostname?: string;
}
@Injectable({
  providedIn: 'root'
})
export class LoggerService {

  clientInfo!: ClientInfo;
  constructor(private logger: NGXLogger, private http: HttpClient, private commonService:CommonService) { }

  logDebug(message: string) {
    this.logger.debug(message);
    this.sendLogToServer('debug', message);
    const userLog = localStorage.getItem('userLogs') || '';
    const updatedUserLog = `${userLog}\n'debug'` + message;
    localStorage.setItem('userLogs', updatedUserLog);
  }

  logInfo(message: string) {
    this.logger.info(message);
    this.sendLogToServer('info', message);
    const userLog = localStorage.getItem('userLogs') || '';
    const updatedUserLog = `${userLog}\n'info'` + message;
    localStorage.setItem('userLogs', updatedUserLog);
  }

  logWarning(message: string) {
    this.logger.warn(message);
    this.sendLogToServer('warn', message);
    const userLog = localStorage.getItem('userLogs') || '';
    const updatedUserLog = `${userLog}\n'warn'$` + message;
    localStorage.setItem('userLogs', updatedUserLog);
  }

  logError(message: string) {
    this.logger.error(message);
    this.sendLogToServer('error', message);
    const userLog = localStorage.getItem('userLogs') || '';
    const updatedUserLog = `${userLog}\n'error'` + message;
    localStorage.setItem('userLogs', updatedUserLog);
  }
  logGlobalError(message: string) {
    this.logger.error(message);
    this.sendLogToServer('UNEXPECTED || GLOBAL ERROR ', message);
    const userLog = localStorage.getItem('userLogs') || '';
    const updatedUserLog = `${userLog}\n'UNEXPECTED || GLOBAL ERROR'` + message;
    localStorage.setItem('userLogs', updatedUserLog);
  }

  private sendLogToServer(level: string, message: string) {
    this.clientInfo = JSON.parse(localStorage.getItem("clientInfo") || "{}") as ClientInfo;
    message = 'IP_Address: \t ' + this.clientInfo.ipAddress + ' \t Hostname: ' + this.clientInfo.hostname + '\t "' + 'UNEXPECTED || GLOBAL ERROR`$`' + message;
    const logEntry = { level, message };

    // Set headers to specify JSON content
    const headers = new HttpHeaders({
      ...this.commonService.headers,
      'Content-Type': 'application/json' });

    // Make the HTTP post request
    this.http.post(this.commonService.LogUrl, logEntry, { headers }).subscribe(
      () => { },
      error => {
        console.error('Error sending log to server:', error);
      }
    );
  }
}

// Somewhere in a central place, like main.ts or a logging service
const originalLog = console.log;
const originalError = console.error;
const originalWarn = console.warn;

const logBuffer: string[] = [];

function saveLogToBuffer(level: string, args: any[]) {
  const timestamp = new Date().toISOString();
  logBuffer.push(`[${timestamp}] [${level.toUpperCase()}] ${args.join(' ')}`);
  localStorage.setItem('userLogs', logBuffer.join('\n'));
}

console.log = (...args: any[]) => {
  saveLogToBuffer('log', args);
  originalLog(...args);
};

console.error = (...args: any[]) => {
  saveLogToBuffer('error', args);
  originalError(...args);
};

console.warn = (...args: any[]) => {
  saveLogToBuffer('warn', args);
  originalWarn(...args);
};
