import httpService from "./httpService";
import urls from "../utils/urls.json";
import apiUrl from "../utils/api";

class AttendanceService {
  static #apiUrl = apiUrl + urls.attendanceRoute;
  static #studentsApiUrl = this.#apiUrl + "getStudents/";
  static #saveAttendanceApiUrl = this.#apiUrl + "saveAttendance/";

  static getStudentsAsync = async (options = {}) => {
    const response = await httpService.get(this.#studentsApiUrl, {
      params: {
        options: options,
      },
    });
    return {
      data: response.data,
      status: response.status,
    };
  };

  static saveAttendance = async (data) => {
    await httpService.post(this.#saveAttendanceApiUrl, data);
  };
}

export default AttendanceService;
