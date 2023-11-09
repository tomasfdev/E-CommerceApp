import { Component, Input } from '@angular/core';
import { ShopParams } from '../Models/shopParams';

@Component({
  selector: 'app-paging-header',
  templateUrl: './paging-header.component.html',
  styleUrls: ['./paging-header.component.scss'],
})
export class PagingHeaderComponent {
  @Input() pageIndex?: number;
  @Input() pageSize?: number;
  @Input() totalCount?: number;
}
