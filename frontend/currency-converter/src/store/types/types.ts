export type CurrencyCode = 'PLN' | 'JPY';

export type CurrencySymbol = 'zł' | '¥';

export type ExchangeRate = {
    from: number,
    to: number,
    rate: number,
    date: Date,
};

export type CurrencyServiceData = {
    symbol: CurrencySymbol,
    code: CurrencyCode,
    isActive: boolean,
} & ExchangeRate;

export type Currency = {
    id: string,
    name: string,
    description: string,
} & CurrencyServiceData;

export type Currencies = Currency[];