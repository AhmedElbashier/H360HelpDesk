import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UserLogService {
  private readonly LOG_KEY = 'userLogs';

  constructor() {
    this.clearLogsDaily();
  }

  private clearLogsDaily() {
    const lastClearTime = localStorage.getItem('lastLogClearTime');
    const currentTime = new Date().getTime();
    const twentyFourHours = 24 * 60 * 60 * 1000; // 24 hours in milliseconds

    if (!lastClearTime || currentTime - Number(lastClearTime) >= twentyFourHours) {
      localStorage.removeItem(this.LOG_KEY);
      localStorage.setItem('lastLogClearTime', currentTime.toString());
    }
  }

  appendLog(log: string) {
    const existingLogs = localStorage.getItem(this.LOG_KEY) || '';
    const updatedLogs = `${existingLogs}\n${log}`;
    localStorage.setItem(this.LOG_KEY, updatedLogs);
  }

  getLogs(): string {
    return localStorage.getItem(this.LOG_KEY) || '';
  }
}
