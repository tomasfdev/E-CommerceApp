import { Component } from '@angular/core';
import { AccountService } from 'src/app/account/account.service';
import { BasketService } from 'src/app/basket/basket.service';
import { BasketItem } from 'src/app/shared/Models/basket';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
})
export class NavBarComponent {
  constructor(public basketService: BasketService, public accountService: AccountService) {}

  getCount(items: BasketItem[]) {
    return items.reduce((sum, item) => sum + item.quantity, 0); //adiciona a quantidade "item.quantity" de cada "item" dentro do array/basket a "sum", dando valor inicial de "sum" de 0
  }
}
