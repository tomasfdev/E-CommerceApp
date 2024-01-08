import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, delay, finalize } from 'rxjs';
import { BusyService } from '../services/busy.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyService: BusyService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (request.url.includes('emailexists') || request.method === 'POST' && request.url.includes('order') || request.method === 'DELETE' && request.url.includes('basket')) {  //no LOADING spinner(busyService.busy())
      return next.handle(request);
    }
    
    this.busyService.busy(); //antes de fazer request à API faz LOADING spinner
    return next.handle(request).pipe(
      delay(1000),
      finalize(() => this.busyService.idle())  //dps request hide LOADING
    );
  } 
}
