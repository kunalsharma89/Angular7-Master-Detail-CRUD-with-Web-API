import { Component, OnInit } from '@angular/core';
import { OrderService } from 'src/app/shared/order.service';
import { NgForm } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog'
import { OrderItemsComponent } from '../order-items/order-items.component';
import { CustomerService } from 'src/app/shared/customer.service';
import { Customer } from 'src/app/shared/customer.model';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styles: [
  ]
})
export class OrderComponent implements OnInit {

  customerList:Customer[];
  isValid:boolean = true;
  constructor(public service:OrderService,
              private dialog:MatDialog, 
              private customerService: CustomerService,
              private toaster:ToastrService,
              private router:Router,
              private currentRoute: ActivatedRoute) { }

  ngOnInit(): void {
    let orderId = this.currentRoute.snapshot.paramMap.get("id");
    if(orderId == null){
      this.resetForm();
    }
    else{
        this.service.GetOrder(parseInt(orderId)).then(res=>{
         
            this.service.formData = res.order;
            this.service.orderItems = res.orderItems;
        });
    } 
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
      if(orderItemId!=null){
        this.service.formData.DeletedItemIds += orderItemId + ",";
      }
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
      GarndTotal:0,
      DeletedItemIds:''
    }
    this.service.orderItems = [];
  }

  validateForm(){
    this.isValid = true;
    if(this.service.formData.CustomerId == 0){
      this.isValid = false;
    } else if(this.service.formData.PaymentMethod == ''){
      this.isValid = false;
    } else if(this.service.orderItems.length == 0){
      this.isValid = false;
    }
    return this.isValid;
  }

  onSubmit(form:NgForm){
    if(this.validateForm()){
      this.service.SaveOrderData().subscribe(res=> {
        this.resetForm();
        this.toaster.success("Order saved successfully.","Restarunt App");
        this.router.navigate(["/orders"]);
      });
    }
  }
}
