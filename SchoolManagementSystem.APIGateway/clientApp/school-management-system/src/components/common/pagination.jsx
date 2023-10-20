import React from "react";
import _ from "lodash";
import PropTypes from "prop-types";

const Pagination = (props) => {
  const { pageSize, currentPage, itemsCount, onPageChange } = props;
  const pagesCount = Math.ceil(itemsCount / pageSize);
  const pages = _.range(1, pagesCount + 1);

  return (
    pagesCount > 1 && (
      <nav aria-label="...">
        <ul className="pagination">
          {pages.map((page) => (
            <li
              className={`page-item ${currentPage === page && "active"}`}
              key={page}
            >
              <button className="page-link" onClick={() => onPageChange(page)}>
                {page}
              </button>
            </li>
          ))}
        </ul>
      </nav>
    )
  );
};

Pagination.propTypes = {
  pageSize: PropTypes.number.isRequired,
  currentPage: PropTypes.number.isRequired,
  itemsCount: PropTypes.number.isRequired,
  onPageChange: PropTypes.func.isRequired,
};

export default Pagination;
