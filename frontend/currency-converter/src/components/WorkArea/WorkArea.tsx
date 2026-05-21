import styles from './WorkArea.module.scss';
import {Heading} from "../Heading/Heading.tsx";
import {SelectionGroup} from "../SelectionGroup/SelectionGroup.tsx";
import type {Currencies} from "../../store/types/types.ts";

type WorkAreaProps = {
    currencies?: Currencies,
}

export const WorkArea = (
    {
        currencies
    }: WorkAreaProps
) => {
    return (
        <div className={styles.workArea}>
            <Heading/>
            <SelectionGroup currencies={currencies}/>
        </div>
    );
}