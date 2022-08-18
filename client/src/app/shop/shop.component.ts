import { Component, OnInit } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {

  products:IProduct[];
  brands:IBrand[];
  types:IType[];
  totalCount:number;
  shopParams=new ShopParams();
  sortOptions=[
    {name:'Alphabetical',value:'name'},
    {name:'Price: low to high',value:'priceAsc'},
    {name:'Price: high to low',value:'priceDesc'}
  ];
  constructor(private shopService:ShopService) { }

  ngOnInit(): void {
   this.getBrands();
   this.getTypes();
   this.getProducts();
   
  }
  getProducts(){
    this.shopService.getProducts(this.shopParams).subscribe(res=>{
      this.products=res.data;
      this.shopParams.pageNumber=res.pageIndex;
      this.shopParams.pageSize=res.pageSize;
      this.totalCount=res.count;
      console.log("-----------------for page------------");
    console.log(this.shopParams.pageNumber);
    console.log(this.shopParams.pageSize);
    console.log(this.totalCount);

    },error=>{
      console.log(error);
    })
  }

  getBrands(){
    this.shopService.getBrands().subscribe(res=>{
      this.brands=[{id:0,name:'All'},...res];
    },error=>{
      console.log(error);
    })
  }

  getTypes(){
    this.shopService.getTypes().subscribe(res=>{
      this.types=[{id:0,name:'All'},...res];
    },error=>{
      console.log(error);
    })
  }

  onBrandSelected(brandId:number){
    this.shopParams.brandId= brandId;
    this.getProducts();
  }

  onTypeSelected(typeId:number){
    this.shopParams.typeId= typeId;
    this.getProducts();
  }
  onSortSelected(sort:string){
    this.shopParams.sort=sort;
    console.log(sort);
    this.getProducts();
  }
  onPageChanged(event:any){
    this.shopParams.pageNumber=event.page;
    this.getProducts();
    
  }

}
