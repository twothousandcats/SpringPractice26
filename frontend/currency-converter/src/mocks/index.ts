import type { Currencies, PriceChanges } from '../models/types.ts';
import currenciesData from './currencies.json';
import priceChangesData from './price_changes.json';

export const currencies: Currencies = currenciesData;
export const priceChanges: PriceChanges = priceChangesData as PriceChanges;