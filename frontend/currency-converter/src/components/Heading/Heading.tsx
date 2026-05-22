import styles from "./Heading.module.scss";
import type {Currency} from "../../models/types.ts";
import {formatDateTime} from "../../utils/functions.ts";

type HeadingProps = {
    amount: number;
    result: number;
    fromCurrency: Currency;
    toCurrency: Currency;
    updDate: string;
}

export const Heading = (
    {
        amount,
        result,
        fromCurrency,
        toCurrency,
        updDate,
    }: HeadingProps) => {
    return (
        <div className={styles.heading}>
            <p className={styles.from}>{amount} {fromCurrency.name} is</p>
            <p className={styles.to}>{result} {toCurrency.name}</p>
            <p className={styles.date}>{formatDateTime(updDate)}</p>
        </div>
    );
}