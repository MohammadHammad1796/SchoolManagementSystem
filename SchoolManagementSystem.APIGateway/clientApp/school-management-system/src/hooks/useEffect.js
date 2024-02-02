import { useEffect } from "react";

export function useEffectOnInitialRender(effect) {
  useEffect(() => {
    return effect();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
}
