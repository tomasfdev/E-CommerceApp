import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BasketItem } from '../Models/basket';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss'],
})
export class BasketSummaryComponent {
  @Output() addItem = new EventEmitter<BasketItem>(); //output props to make conn with basketComponent.ts
  @Output() removeItem = new EventEmitter<{ id: number; quantity: number }>(); //emits 2 output props, "id" e "quantity" inside obj
  @Input() isBasket = true;

  constructor(public basketService: BasketService) {}

  addBasketItem(item: BasketItem) {
    this.addItem.emit(item);
  }

  removeBasketItem(id: number, quantity = 1) {
    this.removeItem.emit({ id, quantity });
  }
}
