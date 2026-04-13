export class InvoiceValidator {
  chkInvoiceId(invoiceId: string): boolean {
    if (invoiceId.length !== 8) return false;
    if (!/^\d{8}$/.test(invoiceId)) return false;

    const checkKey = [1, 2, 1, 2, 1, 2, 4, 1];
    let totalSum = 0;
    let hasSpecialCase = false;

    for (let i = 0; i < 8; i++) {
      const digit = parseInt(invoiceId.charAt(i), 10);
      const product = checkKey[i] * digit;
      totalSum += Math.floor(product / 10) + (product % 10);

      if (i === 6 && digit === 7) hasSpecialCase = true;
    }

    if (hasSpecialCase) {
      return totalSum % 5 === 0 || (totalSum + 1) % 5 === 0;
    }

    return totalSum % 5 === 0;
  }
}
