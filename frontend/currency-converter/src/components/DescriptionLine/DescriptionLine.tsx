import styles from './DescriptionLine.module.scss';
import type { Currency } from "../../models/types.ts";

const FALLBACK_DESCRIPTION = 'No description available for this currency.';

type DescriptionLineProps = {
    currency: Currency;
}

export const DescriptionLine = ({ currency }: DescriptionLineProps) => {
    return (
        <article className={styles.descriptionLineWrapper}>
            <p className={styles.descriptionLineHeading}>
                {`${currency.name} — ${currency.code} — ${currency.symbol}`}
            </p>
            <p className={styles.descriptionLineText}>
                {currency.description || FALLBACK_DESCRIPTION}
            </p>
        </article>
    );
}