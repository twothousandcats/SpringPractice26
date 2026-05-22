import styles from "./Heading.module.scss";

type HeadingProps = {
    from: string;
    to: string;
}

export const Heading = ({from, to}: HeadingProps) => {
    console.log(from);
    console.log(to);
    return (
        <div className={styles.heading}>
            <p className={styles.from}>{from}</p>
            <p className={styles.to}>{to}</p>
            <p className={styles.date}>Currency converter</p>
        </div>
    );
}