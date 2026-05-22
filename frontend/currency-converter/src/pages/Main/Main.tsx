import styles from "./Main.module.scss";
import {Base} from "../../components/Base/Base.tsx";

export const Main = () => {
    return (
        <main className={styles.main}>
            <Base/>
        </main>
    );
}