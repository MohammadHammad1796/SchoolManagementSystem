import httpService from "./httpService";
import urls from "../utils/urls.json";
import apiUrl from "../utils/api";

class AdmissionService {
  static #apiUrl = apiUrl + urls.registrationRoute;
  static #enrollApiUrl = this.#apiUrl + "enroll/";
  static #specializesApiUrl = this.#apiUrl + "specializes/";
  static #studentsApiUrl = this.#apiUrl + "getStudents/";
  static #admitApiUrl = this.#apiUrl + "admit/";
  static #isAcceptedApiUrl = this.#apiUrl + "isAccepted/";

  static enrollAsync = async (data) => {
    return await httpService.post(this.#enrollApiUrl, data);
  };

  static getSpecializesAsync = async () => {
    const response = await httpService.get(this.#specializesApiUrl);
    return {
      data: response.data,
      status: response.status,
    };
  };

  static getStudentsAsync = async (options = {}) => {
    const query = JSON.parse(JSON.stringify(options));
    const filters = query.filters;
    if (filters) {
      if (!filters.status) delete query.filters.status;
      if (!filters.specializeId) delete query.filters.specializeId;
    }

    const response = await httpService.get(this.#studentsApiUrl, {
      params: {
        options: query,
      },
    });
    return {
      data: response.data,
      status: response.status,
    };
  };

  static admitStudentAsync = async (id, isAccepted) => {
    await httpService.post(this.#admitApiUrl + id, { isAccepted });
  };

  static isAcceptedAsync = async () => {
    const response = await httpService.get(this.#isAcceptedApiUrl);
    return {
      data: response.data,
      status: response.status,
    };
  };
}

export default AdmissionService;
