import { Component, OnInit } from '@angular/core';
import { ShopService } from '../shop.service';
import { Product } from 'src/app/shared/Models/product';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent implements OnInit {
  product?: Product;

  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.getProduct();
  }

  getProduct() {
    const id = this.activatedRoute.snapshot.paramMap.get('id'); //vai buscar o id passado como QUERY STRING/ROUTE PARAMETERS
    if (id) this.shopService.getProduct(+id).subscribe({
      next: response => this.product = response,
      error: error => console.log(error)
    })
  }
}
