import styles from './DescriptionLine.module.scss';
import type {Currencies} from "../../store/types/types.ts";

type DescriptionLineProps = {
    name: string,
    code: string,
    symbol: string,
    description: string,
}

export const DescriptionLine = (
    {
        name,
        code,
        symbol,
        description,
    }: DescriptionLineProps
) => {
    return (
        <>
            <p className={styles.descriptionLineHeading}>{`${name} - ${code} - ${symbol}`}</p>
            <p className={styles.descriptionLineText}>{description}</p>
        </>
    );
}