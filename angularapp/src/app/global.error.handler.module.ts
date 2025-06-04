import { ErrorHandler, Injectable } from '@angular/core';
import { NGXLogger } from 'ngx-logger';
import { LoggerService } from './services/logger.service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private logger: NGXLogger, private loggerService: LoggerService) { }

  handleError(error: any): void {
    this.logger.error('An unexpected error occurred:', error);
    this.loggerService.logGlobalError(error);
    const userLog = localStorage.getItem('userLogs') || '';
    const updatedUserLog = `${userLog}\n${error}`;
    localStorage.setItem('userLogs', updatedUserLog);
    // You can also send the error to a server or perform other actions here.
  }
}
