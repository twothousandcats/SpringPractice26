import styles from "./Base.module.scss";
import {WorkArea} from "../WorkArea/WorkArea.tsx";
import {DescriptionGroup} from "../DescriptionGroup/DescriptionGroup.tsx";
import {useConverter} from "../../hooks/useConverter.ts";

export const Base = () => {
    const {
        currencies,
        from,
        to,
        amount,
        result,
        fromCurrency,
        toCurrency,
        dateTime,
        setFrom,
        setTo,
        setAmount,
        swap,
    } = useConverter();

    return (
        <div className={`${styles.base} ${styles.container}`}>
            <WorkArea
                currencies={currencies}
                from={from}
                to={to}
                amount={amount}
                result={result}
                fromCurrency={fromCurrency}
                toCurrency={toCurrency}
                dateTime={dateTime}
                onFromChange={setFrom}
                onToChange={setTo}
                onAmountChange={setAmount}
                onSwap={swap}
            />
            <DescriptionGroup
                key={`${from}/${to}`}
                from={fromCurrency}
                to={toCurrency}
            />
        </div>
    )
}