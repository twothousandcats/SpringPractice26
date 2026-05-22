import styles from "./Select.module.scss";
import {SelectList} from "../SelectList/SelectList.tsx";
import type {Currencies} from "../../models/types.ts";
import {formatNumber} from "../../utils/functions.ts";

type SelectProps = {
    currencies: Currencies;
    selected: string;
    onSelect: (code: string) => void;
    value: number;
    onValueChange?: (value: number) => void;
    readOnly?: boolean;
    containerTestId?: string;
    inputTestId?: string;
}

export const Select = (
    {
        currencies,
        selected,
        onSelect,
        value,
        onValueChange,
        containerTestId,
        inputTestId,
        readOnly = false,
    }: SelectProps) => {
    return (
        <div className={styles.select} data-testid={containerTestId}>
            <input
                type="number"
                data-testid={inputTestId}
                value={readOnly ? formatNumber(value) : value}
                readOnly={readOnly}
                onChange={
                    readOnly
                        ? undefined
                        : (event) => onValueChange?.(Number(event.target.value))
                }
            />
            <div className={styles.separator}></div>
            <SelectList currencies={currencies} selected={selected} onSelect={onSelect}/>
        </div>
    );
}