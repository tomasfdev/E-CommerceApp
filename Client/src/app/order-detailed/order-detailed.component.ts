import { Component, OnInit } from '@angular/core';
import { Order } from '../shared/Models/order';
import { OrdersService } from '../orders/orders.service';
import { ActivatedRoute, Route } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss'],
})
export class OrderDetailedComponent implements OnInit {
  order?: Order;
  constructor(private orderService: OrdersService, private route: ActivatedRoute,
    private bcService: BreadcrumbService) {
      this.bcService.set('@OrderDetailed', ' ');
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    id && this.orderService.getOrderDetailed(+id).subscribe({
      next: response => {
        this.order = response;
        this.bcService.set('@OrderDetailed', `Order# ${response.id} - ${response.status}`);
      }
    })
  }
}
