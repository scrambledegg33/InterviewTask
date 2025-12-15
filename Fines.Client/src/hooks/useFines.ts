import { useEffect, useState } from "react";
import { Fine } from "../types/fine";
import axios from "axios";

const API_URL = new URL("http://localhost:5200/api/Fines");

export function useFines(fineType?: string | undefined, fineDate?: Date | null, vehicleReg?: string | undefined) {
  const [fines, setFines] = useState<Fine[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchFines = async () => {
      setLoading(true);
      setError(null);
      const params: Record<string, string | number | undefined> = {};

      if (typeof fineType === "string" && fineType !== "") {
        const numeric = Number(fineType);
        if (!Number.isNaN(numeric)) {
          params.fineType = numeric;
        } 
      }

      if (fineDate instanceof Date) {
        params.fineDate = fineDate.toISOString();
      }

      if (typeof vehicleReg === "string" && vehicleReg.trim() !== "") {
        params.vehicleReg = vehicleReg.trim();
      }

      try {
        const response = await axios.get(`${API_URL}`, { params });

        if (response.status !== 200) {
          throw new Error(response.statusText);
        }

        const raw = await response.data;
        const fines = raw.map((fine: any) => ({
          ...fine,
          fineDate: new Date(fine.fineDate),
        }));

        setFines(fines);
      } catch (err) {
        console.error(err);
        setError("Failed to fetch fines");
      } finally {
        setLoading(false);
      }
    };

    fetchFines();
  }, [fineType, fineDate, vehicleReg]);

  return { fines, loading, error };
}
