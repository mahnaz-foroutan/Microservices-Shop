import * as cuid from 'cuid';

export interface BasketItem {
    productId: string;
    productName: string;
    price: number;
    quantity: number;
    pictureUrl: string;
    brand: string;
    type: string;
    discountAmount?:number;
}

export interface Basket {
    buyerEmail?:string;
    id: string;
    items: BasketItem[];
    clientSecret?: string;
    paymentIntentId?: string;
    deliveryMethodId?: number;
    shippingPrice: number;
    Subtotal?: number;
    CouponTotal?:number;
}

export class Basket implements Basket {
    id = cuid();
    items: BasketItem[] = [];
    shippingPrice = 0;
}

export interface BasketTotals {
    shipping: number;
    subtotal: number;
    total: number;
    CouponTotal?:number;
}