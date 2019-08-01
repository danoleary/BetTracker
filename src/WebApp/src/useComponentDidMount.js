import React, { useState, useEffect } from 'react';

export default function useComponentDidMount(onMounted) {
    const [mounted, setMounted] = useState(false)
    useEffect(
      () => {
        setMounted(true)
   
        onMounted()
      },
      [mounted]
    )
  }