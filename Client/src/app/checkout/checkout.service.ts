import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { DeliveryMethod } from '../shared/Models/deliveryMethod';
import { map } from 'rxjs';
import { Order, OrderToCreate } from '../shared/Models/order';

@Injectable({
  providedIn: 'root',
})
export class CheckoutService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getDeliveryMethods() {
    return this.http
      .get<DeliveryMethod[]>(this.apiUrl + 'order/deliveryMethods')
      .pipe(
        map((response) => {
          return response.sort((a, b) => b.price - a.price); //return sorted in price order, expensive first
        })
      );
  }

  createOrder(order: OrderToCreate) {
    return this.http.post<Order>(this.apiUrl + 'order', order);
  }
}
