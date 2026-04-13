export interface AppResult<T> {
  success: boolean;
  code: number;
  message: string;
  data: T;
}

export interface UserCredit {
  id: number;
  userId: number;
  smsName: string;
  amount: number;
  createTime: string;
}

export interface CreateTradeTokenRequest {
  userId: number;
  hostId: number;
  pointAmount: string;
  carruerNum?: string;
  email: string;
  phone?: string;
  companyTitle?: string;
  companyTaxId?: string;
  companyAddr?: string;
  invoiceType: string;
}

export interface CreateTradeTokenResponse {
  merchantID: string;
  merchantTradeNo: string;
  token: string;
  tokenExpireDate: string;
}

export interface CreatePaymentResponse {
  threeDURL: string;
}
