import styles from "./SelectionGroup.module.scss";
import {Select} from "../Select/Select.tsx";
import type {Currencies} from "../../models/types.ts";

type SelectGroupProps = {
    currencies: Currencies;
    from: string;
    to: string;
    amount: number;
    result: number;
    onFromChange: (code: string) => void;
    onToChange: (code: string) => void;
    onAmountChange: (value: number) => void;
    onSwap: () => void;
}

export const SelectionGroup = (
    {
        currencies,
        from,
        to,
        amount,
        result,
        onFromChange,
        onToChange,
        onAmountChange,
        onSwap,
    }: SelectGroupProps) => {
    return (
        <div className={styles.selectionGroup}>
            <Select
                currencies={currencies}
                selected={from}
                onSelect={onFromChange}
                value={amount}
                onValueChange={onAmountChange}
                containerTestId="from-select"
                inputTestId="amount-input"
            />
            <button type="button" onClick={onSwap}>Swap</button>
            <Select
                currencies={currencies}
                selected={to}
                onSelect={onToChange}
                value={result}
                readOnly
                containerTestId="to-select"
                inputTestId="result-input"
            />
        </div>
    );
}