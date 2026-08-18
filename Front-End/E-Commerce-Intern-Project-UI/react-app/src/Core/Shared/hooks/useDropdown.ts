import { useState } from 'react';

export const useDropdown = (initialState = false) => {
  const [isOpen, setIsOpen] = useState(initialState);

  const toggle = () => setIsOpen(!isOpen);
  const open = () => setIsOpen(true);
  const close = () => setIsOpen(false);

  return {
    isOpen,
    toggle,
    open,
    close,
  };
};

export const useExclusiveDropdowns = (dropdownNames: string[]) => {
  const [openDropdowns, setOpenDropdowns] = useState<Record<string, boolean>>(() =>
    dropdownNames.reduce((acc, name) => ({ ...acc, [name]: false }), {})
  );

  const toggleDropdown = (name: string) => {
    setOpenDropdowns(prev => {
      const newState = { ...prev };
      // Close all other dropdowns
      dropdownNames.forEach(key => {
        newState[key] = key === name ? !prev[key] : false;
      });
      return newState;
    });
  };

  const closeAllDropdowns = () => {
    setOpenDropdowns(dropdownNames.reduce((acc, name) => ({ ...acc, [name]: false }), {}));
  };

  const isDropdownOpen = (name: string) => openDropdowns[name] || false;

  return {
    toggleDropdown,
    closeAllDropdowns,
    isDropdownOpen,
  };
};
