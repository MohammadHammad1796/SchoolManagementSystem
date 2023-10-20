import httpService from "./httpService";
import urls from "../utils/urls.json";
import apiUrl from "../utils/api";

class CourseService {
  static #apiUrl = apiUrl + urls.coursesRoute;
  static #coursesApiUrl = this.#apiUrl + "getAll/";
  static #addApiUrl = this.#apiUrl + "add/";
  static #updateApiUrl = this.#apiUrl + "update/";
  static #getByIdApiUrl = this.#apiUrl + "getById/";
  static #deleteIdApiUrl = this.#apiUrl + "delete/";
  static #getForMySpecializeApiUrl = this.#apiUrl + "getForMySpecialize/";
  static #registerApiUrl = this.#apiUrl + "register/";
  static #isCoursesRegisteredApiUrl = this.#apiUrl + "isRegistered/";
  static #getMineApiUrl = this.#apiUrl + "getMine/";
  static #getStudentCoursesApiUrl = this.#apiUrl + "getStudentCourses/";

  static getCoursesAsync = async (options = {}) => {
    const query = JSON.parse(JSON.stringify(options));
    const filters = query.filters;
    if (filters) {
      if (!filters.specializeId) delete query.filters.specializeId;
    }

    const response = await httpService.get(this.#coursesApiUrl, {
      params: {
        options: query,
      },
    });
    return {
      data: response.data,
      status: response.status,
    };
  };

  static addCourseAsync = async (course) => {
    await httpService.post(this.#addApiUrl, course);
  };

  static updateCourseAsync = async (id, course) => {
    await httpService.put(this.#updateApiUrl + id, course);
  };

  static getByIdAsync = async (id) => {
    const response = await httpService.get(this.#getByIdApiUrl + id);
    return {
      data: response.data,
      status: response.status,
    };
  };

  static deleteAsync = async (id) => {
    await httpService.delete(this.#deleteIdApiUrl + id);
  };

  static getForMySPecializeAsync = async () => {
    const response = await httpService.get(this.#getForMySpecializeApiUrl);
    return {
      data: response.data,
      status: response.status,
    };
  };

  static registerAsync = async (courses) => {
    await httpService.post(this.#registerApiUrl, courses);
  };

  static isCoursesRegisteredAsync = async () => {
    const response = await httpService.get(this.#isCoursesRegisteredApiUrl);
    return {
      data: response.data,
      status: response.status,
    };
  };

  static getMineAsync = async () => {
    const response = await httpService.get(this.#getMineApiUrl);
    return {
      data: response.data,
      status: response.status,
    };
  };

  static getStudentCoursesAsync = async (id) => {
    const response = await httpService.get(this.#getStudentCoursesApiUrl + id);
    return {
      data: response.data,
      status: response.status,
    };
  };
}

export default CourseService;
