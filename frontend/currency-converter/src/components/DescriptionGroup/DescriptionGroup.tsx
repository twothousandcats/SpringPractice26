import styles from './DescriptionGroup.module.scss';
import type {Currency} from "../../store/types/types.ts";
import {DescriptionLine} from "../DescriptionLine/DescriptionLine.tsx";

type DescriptionGroupProps = {
    currency: Currency,
}

export const DescriptionGroup = (
    {currency}: DescriptionGroupProps
) => {
    return (
        <div className={styles.descriptionGroup}>
            <DescriptionLine
                name={currency.name}
                code={currency.code}
                symbol={currency.symbol}
                description={currency.description}/>
        </div>
    );
}