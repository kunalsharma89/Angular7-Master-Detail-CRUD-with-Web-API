import { Component, OnInit } from '@angular/core';
import { OrderService } from 'src/app/shared/order.service';
import { NgForm } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog'
import { OrderItemsComponent } from '../order-items/order-items.component';
import { CustomerService } from 'src/app/shared/customer.service';
import { Customer } from 'src/app/shared/customer.model';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styles: [
  ]
})
export class OrderComponent implements OnInit {

  customerList:Customer[];
  constructor(public service:OrderService, private dialog:MatDialog, private customerService: CustomerService) { }

  ngOnInit(): void {
    this.resetForm();
    this.customerService.getCustomerList().then(res=> this.customerList = res as Customer[]);
  }

  AddOrEditOrderItems(OrderItemIndex, OrderId){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.disableClose= true;
    dialogConfig.width = "50%";
    dialogConfig.data = { OrderItemIndex, OrderId };
    this.dialog.open(OrderItemsComponent, dialogConfig).afterClosed().subscribe(res=>{
      this.updateGrandTotal();
    });
  }

  DeleteOrderItems(orderItemId:number, index:number){
      this.service.orderItems.splice(index,1);
      this.updateGrandTotal();
  }

  updateGrandTotal(){
    this.service.formData.GarndTotal = this.service.orderItems.reduce((prev,curr)=>{
      return prev + curr.Total;
    },0);
    this.service.formData.GarndTotal = parseFloat(this.service.formData.GarndTotal.toFixed(2));
  }

  resetForm(form?:NgForm){
    if(form=null)
      form.resetForm();
    
    this.service.formData={
      OrderNo :Math.floor(100000+Math.random()*900000).toString(),
      OrderId:null,
      CustomerId:0,
      PaymentMethod:"",
      GarndTotal:0
    }
    this.service.orderItems = [];

  }
}
