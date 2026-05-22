import styles from './WorkArea.module.scss';
import {Heading} from "../Heading/Heading.tsx";
import {SelectionGroup} from "../SelectionGroup/SelectionGroup.tsx";
import type {Currencies} from "../../models/types.ts";

type WorkAreaProps = {
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

export const WorkArea = (
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
    }: WorkAreaProps) => {
    return (
        <div className={styles.workArea}>
            <Heading from={from} to={to}/>
            <SelectionGroup
                currencies={currencies}
                from={from}
                to={to}
                amount={amount}
                result={result}
                onFromChange={onFromChange}
                onToChange={onToChange}
                onAmountChange={onAmountChange}
                onSwap={onSwap}
            />
        </div>
    );
}