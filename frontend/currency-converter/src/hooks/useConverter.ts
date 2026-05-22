import {useMemo, useState} from 'react';
import type {Currency} from '../models/types.ts';
import {currencies, priceChanges} from '../mocks';

const findByCode = (code: string): Currency => {
    return currencies.find(
        (currency) => currency.code === code
    ) ?? currencies[0];
}

const firstDifferentCode = (code: string): string => {
    return (currencies.find((currency) => currency.code !== code) ?? currencies[0]).code;
}

export const useConverter = () => {
    const [from, setFromCode] = useState<string>(currencies[0].code); // code
    const [to, setToCode] = useState<string>(currencies[1].code); // code
    const [amount, setAmount] = useState<number>(1);

    const setFrom = (code: string): void => {
        setFromCode(code);
        if (code === to) {
            setToCode(firstDifferentCode(code));
        }
    };

    const setTo = (code: string): void => {
        setToCode(code);
        if (code === from) {
            setFromCode(firstDifferentCode(code));
        }
    };

    const swap = (): void => {
        setFromCode(to);
        setToCode(from);
    };

    const rate = priceChanges[from]?.[to]?.price ?? 0;
    const result = useMemo(() => amount * rate, [amount, rate]);

    const fromCurrency = findByCode(from);
    const toCurrency = findByCode(to);
    const dateTime = priceChanges[from]?.[to]?.dateTime ?? '';

    return {
        currencies,
        from,
        to,
        amount,
        result,
        rate,
        fromCurrency,
        toCurrency,
        setFrom,
        setTo,
        setAmount,
        swap,
        dateTime,
    };
};