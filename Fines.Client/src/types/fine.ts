import { FineType } from "../enum/fineType";

export interface Fine {
    id: number;
    fineNo: string;
    fineDate: Date;
    fineType: FineType;
    customerName: string;
    vehicleRegNo: string;
    vehicleDriverName: string;
}