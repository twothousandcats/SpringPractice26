import styles from "./SelectionGroup.module.scss";
import {Select} from "../Select/Select.tsx";
import type {Currencies} from "../../store/types/types.ts";

type SelectGroupProps = {
    currencies?: Currencies,
}

export const SelectionGroup = (
    {
        currencies,
    }: SelectGroupProps) => {
    return (
        <div className={styles.selectionGroup}>
            <Select currencies={currencies}/>
            <Select currencies={currencies}/>
        </div>
    );
}