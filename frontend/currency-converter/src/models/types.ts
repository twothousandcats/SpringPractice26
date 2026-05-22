export type Currency = {
    code: string;
    name: string;
    description: string;
    symbol: string;
};

export type PriceChange = {
    purchasedCurrencyCode: string;
    paymentCurrencyCode: string;
    price: number;
    dateTime: string;
};

export type Currencies = Currency[];

// priceChanges[from][to]
export type PriceChanges = Record<string, Record<string, PriceChange>>;