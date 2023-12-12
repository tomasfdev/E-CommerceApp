import { Component, OnInit } from '@angular/core';
import { ShopService } from '../shop.service';
import { Product } from 'src/app/shared/Models/product';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { BasketService } from 'src/app/basket/basket.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent implements OnInit {
  product?: Product;
  quantity = 1;
  quantityInBasket = 0;

  constructor(
    private shopService: ShopService,
    private activatedRoute: ActivatedRoute,
    private bcService: BreadcrumbService,
    private basketService: BasketService
  ) {
    bcService.set('@productDetails', ' ');
  }

  ngOnInit(): void {
    this.getProduct();
  }

  getProduct() {
    const id = this.activatedRoute.snapshot.paramMap.get('id'); //vai buscar o id passado como QUERY STRING/ROUTE PARAMETERS
    if (id)
      this.shopService.getProduct(+id).subscribe({
        next: (response) => {
          this.product = response;
          this.bcService.set('@productDetails', this.product.name);
          this.basketService.basketSource$.pipe(take(1)).subscribe({  //access to basket
            next: (basket) => {
              const item = basket?.items.find(item => item.id === +id); //verifica se tem o item escolhido na front end dentro do basket
              if (item) {
                this.quantity = item.quantity;
                this.quantityInBasket = item.quantity;
              }
            },
          });
        },
        error: (error) => console.log(error),
      });
  }

  incrementQuantity() {
    this.quantity++;
  }

  decrementQuantity() {
    if (this.quantity > 0) this.quantity--;
  }

  updateBasket() {
    if (this.product) { //verifica se tem product 
      if (this.quantity > this.quantityInBasket) {  //verifica se quantidade do product é maior do que quantidade do product no basket, se sim, há items para add ao basket
        const itemsToAdd = this.quantity - this.quantityInBasket; //nº items deste "product" para add ao basket
        this.quantityInBasket += itemsToAdd;
        this.basketService.addItemToBasket(this.product, itemsToAdd);
      } else {
        const itemsToRemove = this.quantityInBasket - this.quantity;  //nº items deste "product" para remover ao basket
        this.quantityInBasket -= itemsToRemove;
        this.basketService.removeItemFromBasket(this.product.id, itemsToRemove);
      }
    }
  }

  get buttonText() {
    return this.quantityInBasket === 0 ? 'Add to basket' : 'Update basket';
  }
}
