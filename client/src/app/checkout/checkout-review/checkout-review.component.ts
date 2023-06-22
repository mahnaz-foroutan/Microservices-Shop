import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { ActivatedRoute, Router } from '@angular/router';
@Component({
  selector: 'app-checkout-review',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss']
})
export class CheckoutReviewComponent {
  @Input() appStepper?: CdkStepper;
  returnUrl: string;
  constructor(private basketService: BasketService, private toastr: ToastrService,private router: Router, private activatedRoute: ActivatedRoute) {

    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || '/order'
  }

  createPaymentIntent() {
    this.basketService.createPaymentIntent().subscribe({
      next: (r) => {if(r==true)
      this.router.navigateByUrl(this.returnUrl)
      }
    })
  }

}
