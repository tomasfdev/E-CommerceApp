import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Order } from '../shared/Models/order';

@Injectable({
  providedIn: 'root',
})
export class OrdersService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getOrdersForUser() {
    return this.http.get<Order[]>(this.apiUrl + 'order');
  }
  getOrderDetailed(id: number) {
    return this.http.get<Order>(this.apiUrl + 'order/' + id);
  }
}
