import styles from "./Select.module.scss";
import {SelectList} from "../SelectList/SelectList.tsx";
import type {Currencies} from "../../models/types.ts";
import {formatNumber} from "../../utils/functions.ts";
import {TriangleDownIcon} from "../Icons/TriangleDownIcon.tsx";
import {useState} from "react";

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
    const [isOpen, setIsOpen] = useState(false);
    console.log(isOpen);

    return (
        <div className={styles.select}
             data-testid={containerTestId}>
            <input
                type="number"
                className={styles.input}
                min={0}
                readOnly={readOnly}
                data-testid={inputTestId}
                value={
                    readOnly
                        ? formatNumber(value)
                        : value
                }
                onChange={
                    readOnly
                        ? undefined
                        : (event) => onValueChange?.(Number(event.target.value))
                }
            />
            <div className={styles.separator}></div>
            <div
                className={styles.selected}
                onClick={() => {
                    setIsOpen((prev) => !prev);
                }}>
                {selected}
            </div>
            <TriangleDownIcon/>
            <SelectList
                currencies={currencies}
                selected={selected}
                isOpen={isOpen}
                onSelect={(code) => {
                    onSelect(code);
                    setIsOpen(false);
                }}
            />
        </div>
    );
}