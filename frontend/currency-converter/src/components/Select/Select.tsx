import styles from "./Select.module.scss";
import {SelectList} from "../SelectList/SelectList.tsx";
import type {Currencies} from "../../store/types/types.ts";

type SelectProps = {
    currencies?: Currencies,
}

export const Select = (
    {
        currencies,
    }: SelectProps) => {
    return (
        <div className={styles.select}>
            <input
                type="number"
                value={1}
            />
            <div className={styles.separator}></div>
            <SelectList currencies={currencies}/>
        </div>
    );
}